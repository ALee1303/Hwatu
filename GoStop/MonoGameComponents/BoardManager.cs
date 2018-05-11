using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using MG_Library;
using GoStop;
using GoStop.Card;
using GoStop.Minhwatu;
using GoStop.MonoGameComponents.Drawables;
using GoStop.Collection;

namespace GoStop.MonoGameComponents
{
    // TODO: Change to CardManager and move boardInitialization to board
    public class BoardManager : DrawableGameComponent
    {
        private IBoard _board;
        private IMainPlayer _mainPlayer;

        private CardFactory cardFactory;
        private Dictionary<Month, List<DrawableCard>> fieldCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> handCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> collectedCards;
        private Dictionary<IHanafudaPlayer, List<SpecialCards>> specialCollected;


        protected Dictionary<IHanafudaPlayer, int> scoreBoard;

        public IMainPlayer MainPlayer { get => _mainPlayer; }
        public IHanafudaPlayer CurrentPlayer { get => _board.CurrentPlayer; }
        public HanafudaController Controller { get => _mainPlayer.Controller; }

        private Sprite2D loadedOutline;

        public BoardManager(Game game) : base(game)
        {
            Game.Services.AddService<BoardManager>(this);
            cardFactory = new CardFactory(Game);
            fieldCards = new Dictionary<Month, List<DrawableCard>>();
            SetupField();
            handCards = new Dictionary<IHanafudaPlayer, List<DrawableCard>>();
            collectedCards = new Dictionary<IHanafudaPlayer, List<DrawableCard>>();
            specialCollected = new Dictionary<IHanafudaPlayer, List<SpecialCards>>();

            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
        }

        private void SetupField()
        {
            for (int i = 0; i < 12; i++)
            {
                Month key = (Month)i;
                fieldCards.Add(key, new List<DrawableCard>());
            }
        }

        public void StartMinhwatuGameVsCPU()
        {
            if (_board != null)
                new ArgumentException("Must Unsubscribe Board first");
            _board = new MinhwatuBoard(this);
            SubscribeToLoadedBoard();
            if (_board.IsNewPlayer(MainPlayer))
                AddPlayerToBoard(MainPlayer);
            Player cpu = new Player();
            AddPlayerToBoard(cpu);
            _board.StartGame();
        }

        /// <summary>
        /// Start game on board that is ready
        /// </summary>
        public void RestartLoadedBoard()
        {
            // TODO: Add case for detecting board's status and
            // Should not work when game is in progress or Drawables are not ready.
        }

        public void OnJoinBoard(IHanafudaPlayer player)
        {
            if (player is IMainPlayer)
                _mainPlayer = (IMainPlayer)player;
            if (_board != null)
                AddPlayerToBoard(player);
        }

        public void OnExitBoard(IHanafudaPlayer player)
        {
            // TODO: for multiple players
        }

        private void AddPlayerToBoard(IHanafudaPlayer player)
        {
            _board.SubscribePlayer(player);
            handCards.Add(player, new List<DrawableCard>());
            collectedCards.Add(player, new List<DrawableCard>());
            specialCollected.Add(player, _board.PrepareSpecialCollection(player));
            specialCollected[player].ForEach(
                (collection) => ((CardCollection)collection).CollectionChanged += collection_Changed);

            scoreBoard.Add(player, 0);

            ((Player)player).CardPlayed += player_CardPlayed;
            ((Player)player).MouseOverCard += player_MouseOverCard;
        }

        private void SubscribeToLoadedBoard()
        {
            Board board = (Board)_board;
            board.NewPlayerTurn += board_NewPlayerTurn;
            board.AllPlayerRemoved += board_AllPlayerRemoved;
            board.CardsDealt += board_CardsDealt;
            board.CardsOnField += board_CardsOnField;
        }




        #region GameComponent

        public override void Initialize()
        {
            base.Initialize();
            cardFactory.Initialize();
        }

        public override void Update(GameTime gameTime)
        { }

        public override void Draw(GameTime gameTime)
        {
            DrawHands();
            // TODO: Location Logic for Collected
            //DrawCollected();
            DrawField();
            if (loadedOutline != null)
                loadedOutline.Draw();
        }

        private void DrawHands()
        {
            handCards.Keys.ToList().ForEach(key => handCards[key].ForEach(
                card => card.Draw()));
        }

        private void DrawCollected()
        {
            collectedCards.Keys.ToList().ForEach(key => collectedCards[key].ForEach(
                card => card.Draw()));
        }

        private void DrawField()
        {
            fieldCards.Keys.ToList().ForEach(key => fieldCards[key].ForEach(
                card => card.Draw()));
        }

        #endregion

        #region Setting up DrawableCards Methods
        /// <summary>
        /// Reorder deck for new game
        /// </summary>
        /// TODO: sort without creating new Drawable
        private void ResetLoadedBoard()
        {
            DiscardDrawables();
            _board.ResetBoard();
        }

        /// <summary>
        /// Initialize drawables
        /// Called right before every game when all cards are in deck
        /// </summary>
        private IEnumerable<DrawableCard> InitializeRevealedDrawables(IEnumerable<Hanafuda> hanafudas)
        {
            List<DrawableCard> drawables = new List<DrawableCard>();
            foreach (Hanafuda card in hanafudas)
            {
                DrawableCard drawable = cardFactory.ReturnPairedDrawable(card);
                drawable.Initialize();
                drawables.Add(drawable);
            }
            return drawables;
        }

        private IEnumerable<DrawableCard> InitializeTurnedDrawables(IEnumerable<Hanafuda> hanafudas)
        {
            List<DrawableCard> drawables = new List<DrawableCard>();
            foreach (Hanafuda card in hanafudas)
            {
                DrawableCard turnedCard = cardFactory.ReturnTurnedDrawable(card);
                turnedCard.Initialize();
                drawables.Add(turnedCard);
            }
            return drawables;
        }

        private void DiscardDrawables()
        {
            fieldCards.Keys.ToList().ForEach(key => fieldCards[key].Clear());
            handCards.Keys.ToList().ForEach(key => handCards[key].Clear());
            collectedCards.Keys.ToList().ForEach(key => collectedCards[key].Clear());
        }
        #endregion

        #region Board Interaction


        protected virtual void board_NewPlayerTurn()
        {
            List<DrawableCard> hand = handCards[CurrentPlayer];
            CurrentPlayer.PlayCard(hand);
        }

        protected virtual void board_CardsDealt(object sender, DealCardEventArgs args)
        {
            IEnumerable<Hanafuda> cards = args.Cards;
            IHanafudaPlayer owner = args.Player;
            IEnumerable<DrawableCard> returnedDrawables = new List<DrawableCard>();
            if (owner == MainPlayer)
                returnedDrawables = InitializeRevealedDrawables(cards);
            else
                returnedDrawables = InitializeTurnedDrawables(cards);
            foreach (DrawableCard drawable in returnedDrawables)
            {
                PlaceCardOnHand(owner, drawable);
            }
        }

        protected virtual void board_CardsOnField(object sender, DealCardEventArgs args)
        {
            IEnumerable<Hanafuda> cards = args.Cards;
            IEnumerable<DrawableCard> returnedDrawables = InitializeRevealedDrawables(cards);
            foreach (DrawableCard drawable in returnedDrawables)
            {
                PlaceCardOnField(drawable);
            }
        }

        protected virtual void board_AllPlayerRemoved()
        {
            
        }

        #endregion

        #region Player EventHandler

        protected virtual void player_MouseOverCard(object sender, PlayerEventArgs args)
        {
            //check for safe case to see if it was player triggering the event
            var mainP = (IMainPlayer)sender;
            if (mainP == null || mainP != MainPlayer)
                return;
            var toOutline = args.Selected;
            if (toOutline != null)
            {
                loadedOutline = cardFactory.RetrieveOutline();
                loadedOutline.Position = toOutline.Position;
            }
            else // destroy loaded outline
            {
                cardFactory.RemoveOutline();
                loadedOutline = null;
            }
        }

        protected virtual void player_CardPlayed(object sender, PlayerEventArgs args)
        {
            // check if Drawable is valid
            var player = (IHanafudaPlayer)sender;
            var played = args.Selected;
            if (played == null || player == null)
                new ArgumentException("Wrong call to player_CardPlayed");
            RemoveCardFromHand(player, played);
            Month idx = played.Card.Month;
            
            
            PlayResult(played).Start();
        }
        #endregion


        #region Card Played

        private async Task PlayResult(DrawableCard played, float delay = 1.0f)
        {
            Month playedMonth = played.Card.Month;            
            RemoveCardFromHand(CurrentPlayer, played);
            PlaceCardOnField(played);
            // place card on field from deck
            DrawableCard drawnCard = await PlayFromDeck();
            Month drawnMonth = drawnCard.Card.Month;
            // and check if player has pooped
            if (drawnMonth == playedMonth)
                return;
            int stack = fieldCards[playedMonth].Count;
            //didnt poop but month had 2 on field from beginning
            if (stack == 3)
            {
                //put played card to collection
                RemoveCardFromField(played);
                PlaceCardOnCollection(CurrentPlayer, played);
                List<DrawableCard> choices = fieldCards[playedMonth];
                PlaceCardOnChoice(choices);
                Task<DrawableCard> selectTask = CurrentPlayer.ChooseCard(choices);
                DrawableCard selected = await selectTask;
                //selected card to collection
                RemoveCardFromField(selected);
                PlaceCardOnCollection(CurrentPlayer, selected);
                //put unselected card back on field
                DrawableCard unselected = choices.Find(x => x != selected);
                PlaceCardOnField(unselected);
            }
            // TODO: make to function
            if (stack == 2 || stack == 4)
            {
                foreach (DrawableCard drawable in fieldCards[playedMonth])
                {
                    RemoveCardFromField(drawable);
                    PlaceCardOnCollection(CurrentPlayer, drawable);
                }
            }
            stack = fieldCards[drawnMonth].Count;
            if (stack == 3)
            {
                //put played card to collection
                RemoveCardFromField(played);
                PlaceCardOnCollection(CurrentPlayer, played);
                List<DrawableCard> choices = fieldCards[drawnMonth];
                PlaceCardOnChoice(choices);
                Task<DrawableCard> selectTask = CurrentPlayer.ChooseCard(choices);
                DrawableCard selected = await selectTask;
                //selected card to collection
                RemoveCardFromField(selected);
                PlaceCardOnCollection(CurrentPlayer, selected);
                //put unselected card back on field
                DrawableCard unselected = choices.Find(x => x != selected);
                PlaceCardOnField(unselected);
            }
            if (stack == 2 || stack == 4)
            {
                foreach (DrawableCard drawable in fieldCards[drawnMonth])
                {
                    RemoveCardFromField(drawable);
                    PlaceCardOnCollection(CurrentPlayer, drawable);
                }
            }
            if (handCards[CurrentPlayer].Count == 0)
                _board.PlayingCount--;
            _board.EndTurn();
        }
        private async Task<DrawableCard> PlayFromDeck(float delay = 2.0f)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            IEnumerable<Hanafuda> drawnCard = DeckCollection.Instance.DrawCard();
            IEnumerable<DrawableCard> drawable = InitializeRevealedDrawables(drawnCard);
            DrawableCard drawn = drawable.First();
            fieldCards[drawn.Card.Month].Add(drawn);
            drawn.Position = new Vector2(GraphicsDevice.Viewport.Width / 2 + 100, GraphicsDevice.Viewport.Height / 2);
            await Task.Delay(TimeSpan.FromSeconds(delay));
            PlaceCardOnField(drawn);
            await Task.Delay(TimeSpan.FromSeconds(delay));
            return drawn;
        }
        
        private void CollectCard(IHanafudaPlayer owner, DrawableCard wonCard)
        {            
            foreach (SpecialCards collection in specialCollected[owner])
                //must preserve drawable reference for later
                collection.OnCardCollected(wonCard);
            _board.CalculatePoint(wonCard.Card);
        }
        #endregion

        #region Card Location Methods

        private void PlaceCardOnHand(IHanafudaPlayer owner, DrawableCard drawable)
        {
            handCards[owner].Add(drawable);
            drawable.Position = GetHandLocation(owner);
        }
        private Vector2 GetHandLocation(IHanafudaPlayer owner)
        {
            int slot = handCards[owner].Count - 1;
            Vector2 position = new Vector2(50.0f * slot, 45.0f);
            position.X += 2 * slot + 30;
            if (owner == MainPlayer)
                position.Y += GraphicsDevice.Viewport.Height - 90.0f;
            return position;
        }

        private void PlaceCardOnField(DrawableCard drawable)
        {
            Month month = drawable.Card.Month;
            fieldCards[month].Add(drawable);
            drawable.Position = GetFieldLocation(month);

        }
        private Vector2 GetFieldLocation(Month month)
        {
            int slot = (int)month;
            int xSlot = (slot % 6);
            int ySlot = (slot / 6);
            int yOffset = GraphicsDevice.Viewport.Height / 2 - 100;
            Vector2 position = new Vector2(80.0f * xSlot, yOffset + 200 * ySlot);
            position.X += 30.0f + ((fieldCards[month].Count - 1) * 6) + (xSlot - 1) * 2;
            return position;
        }


        private void PlaceCardOnChoice(List<DrawableCard> choices)
        {
            for (int i = 0; i < 2; i++)
                choices[i].Position = GetChoiceLocation(i);
        }
        private Vector2 GetChoiceLocation(int count)
        {
            float x = Game.GraphicsDevice.Viewport.Width / 2 - 60;
            float y = Game.GraphicsDevice.Viewport.Height / 2;
            x += 120 * count;
            return new Vector2(x, y);
        }
        
        private void PlaceWonSpecial(DrawableCard card, Type special)
        {
        }
        private Vector2 GetSpecialLocation(Type special)
        {
            return Vector2.One;
        }

        private void PlaceCardOnCollection(IHanafudaPlayer owner, DrawableCard drawable)
        {
            collectedCards[owner].Add(drawable);
            CollectCard(owner, drawable);
        }

        private void RemoveCardFromHand(IHanafudaPlayer player, DrawableCard drawable)
        {
            handCards[player].Remove(drawable);
            if (player != MainPlayer)
                drawable = cardFactory.FlipCard(drawable);
        }
        private void RemoveCardFromField(DrawableCard drawable)
        {
            Month month = drawable.Card.Month;
            fieldCards[month].Remove(drawable);
        }
        #endregion


        protected virtual void collection_Changed(object sender, EventArgs args)
        {
            Type type = sender.GetType();
            SpecialChangedEventArgs specialArg = (SpecialChangedEventArgs)args;
            if (!(type.IsInstanceOfType(typeof(SpecialCards)) || specialArg == null))
                new ArgumentException("Not a Special Collection");

        }
        
        

        #region Score
        public void UpdateScore(int score, IHanafudaPlayer player)
        {
            scoreBoard[player] += score;
        }

        #endregion
    }
}
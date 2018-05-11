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
        private List<DrawableCard> enlargedCards;
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
            enlargedCards = new List<DrawableCard>();
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
            MinhwatuPlayer cpu = new MinhwatuPlayer();
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
            ((Player)player).CardChoose += player_CardChoose;
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


        #region Card Played

        private async Task PlayResult(DrawableCard played, float delay = 1.0f)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            Month month = played.Card.Month;
            RemoveCardFromHand(CurrentPlayer, played);
            PlaceCardOnField(played);
            int stack = fieldCards[month].Count;
            if (stack == 3)
            {
                //put played card to collection
                RemoveCardFromField(played);
                PlaceCardOnCollection(CurrentPlayer, played);
                List<DrawableCard> choices = fieldCards[month];
                for (int i = 0; i < 2; i++)
                    choices[i].Position = GetChoiceLocation(i);
                CurrentPlayer.ChooseCard(choices);
            }
            else
            {
                if (stack == 2 || stack == 4)
                {
                    foreach (DrawableCard drawable in fieldCards[month])
                    {
                        RemoveCardFromField(drawable);
                        PlaceCardOnCollection(CurrentPlayer, drawable);
                    };
                }
                else // no match
                    PlaceCardOnField(played);
                PlayFromDeck();
            }
        }
        private void PlayFromDeck()
        {
            IEnumerable<Hanafuda> drawnCard = DeckCollection.Instance.DrawCard();
            IEnumerable<DrawableCard> drawable = InitializeRevealedDrawables(drawnCard);
            DrawableCard drawn = drawable.First();
            PlayFromDeckResult(drawn.Card.Month, drawn);
        }

        private void PlayFromDeckResult(Month month, DrawableCard played, float delay = 1.0f)
        {
            int stack = fieldCards[month].Count;
            PlaceCardOnField(played);
            if (stack == 2 || stack == 4)
            {
                RemoveCardFromField(played);
                PlaceCardOnCollection(CurrentPlayer, played);
                foreach (DrawableCard drawable in fieldCards[month])
                {
                    RemoveCardFromField(drawable);
                    PlaceCardOnCollection(CurrentPlayer, drawable);
                }
            }
            _board.EndTurn();
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
            DrawEnlarged();
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
        private void DrawEnlarged()
        {
            enlargedCards.ForEach(
                card => card.Draw());
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
            // TODO: Displaying result and ending game, replay button
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
            PlayResult(played);
        }

        protected virtual void player_CardChoose(object sender, PlayerEventArgs args)
        {
            // check if Drawable is valid
            var player = (IHanafudaPlayer)sender;
            var selected = args.Selected;
            var unselected = args.Unselected;
            if (selected == null || unselected == null || player == null)
                new ArgumentException("Wrong call to player_CardChoose");
            enlargedCards.Clear();
            //selected card to collection
            RemoveCardFromField(selected);
            PlaceCardOnCollection(player, selected);
            //put unselected card back on field
            PlaceCardOnField(unselected);
            PlayFromDeck();
        }
        #endregion

        #region Card Location Methods

        private void PlaceCardOnHand(IHanafudaPlayer owner, DrawableCard drawable)
        {
            handCards[owner].Add(drawable);
            drawable.Position = GetHandLocation(owner);
        }

        /// Hardcoded for test screen
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

        private void PlaceCardOnCollection(IHanafudaPlayer owner, DrawableCard drawable)
        {
            collectedCards[owner].Add(drawable);
            CollectCard(owner, drawable);
        }

        private void DisplayWonSpecial(DrawableCard card, Type special)
        {
        }
        private Vector2 GetSpecialLocation(Type special)
        {
            return Vector2.One;
        }

        private Vector2 GetChoiceLocation(int count)
        {
            float x = Game.GraphicsDevice.Viewport.Width  / 2 - 60;
            float y = Game.GraphicsDevice.Viewport.Height / 2;
            x += 120 * count;
            return new Vector2(x, y);
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

        //private void CollectCards(IHanafudaPlayer owner, List<DrawableCard> wonCards)
        //{
        //    foreach (SpecialCards collection in specialCollected[owner])
        //        collection.OnCardsCollected(wonCards);
        //    _board.CalculatePoint();
        //}

        private void CollectCard(IHanafudaPlayer owner, DrawableCard wonCard)
        {
            foreach (SpecialCards collection in specialCollected[owner])
                collection.OnCardCollected(wonCard);
            _board.CalculatePoint();
        }
        

        #endregion

        #region Score
        public void UpdateScore(int score, IHanafudaPlayer player)
        {
            scoreBoard[player] += score;
        }

        #endregion
    }
}
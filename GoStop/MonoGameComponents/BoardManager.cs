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
    // TODO: separate into multiple types of board for GoStop and Minhwatu
    // Currently only supports Minhwatu 1p 1v1 with CPU
    public class BoardManager : DrawableGameComponent
    {
        private IBoard _board;
        private IMainPlayer _mainPlayer;

        private CardFactory cardFactory;
        private List<DrawableCard> deckCards;
        private Dictionary<Month, List<DrawableCard>> fieldCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> handCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> collectedCards;
        private List<DrawableCard> selectableCards;
        private DrawableCard selectedCard;

        public IMainPlayer MainPlayer { get => _mainPlayer; }
        public IHanafudaPlayer CurrentPlayer { get => _board.CurrentPlayer; }
        public HanafudaController Controller { get => _mainPlayer.Controller; }

        private Sprite2D loadedOutline;

        public BoardManager(Game game) : base(game)
        {
            Game.Services.AddService<BoardManager>(this);
            cardFactory = new CardFactory(Game);
            deckCards = new List<DrawableCard>();
            fieldCards = new Dictionary<Month, List<DrawableCard>>();
            SetupField();
            handCards = new Dictionary<IHanafudaPlayer, List<DrawableCard>>();
            collectedCards = new Dictionary<IHanafudaPlayer, List<DrawableCard>>();
            selectableCards = new List<DrawableCard>();
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

        private void SubscribeToLoadedBoard()
        {
            Board board = (Board)_board;
            board.NewPlayerTurn += board_NewPlayerTurn;
            board.AllPlayerRemoved += board_AllPlayerRemoved;
            board.CardsDealt += board_CardsDealt;
            board.CardsOnField += board_CardsOnField;
            board.MultipleMatch += board_MultipleMatch;
        }


        private void AddPlayerToBoard(IHanafudaPlayer player)
        {
            _board.SubscribePlayer(player);
            handCards.Add(player, new List<DrawableCard>());
            collectedCards.Add(player, new List<DrawableCard>());
            ((Player)player).CardPlayed += player_CardPlayed;
            ((Player)player).MouseOverCard += player_MouseOverCard;
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

        private void CardSelectable()
        {
            foreach (DrawableCard selectable in selectableCards)
            {
                // TODO: Mouseover highlight logic
                if (Controller.IsMouseOverCard(selectable) &&
                    Controller.IsLeftMouseClicked())
                    OnCardSelected(selectable);
            }
        }

        protected virtual void OnCardSelected(DrawableCard selected)
        {
            selectedCard = selected;
        }

        #region GameComponent

        public override void Initialize()
        {
            base.Initialize();
            cardFactory.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            if (selectableCards.Count > 0)
                CardSelectable();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawHands();
            // TODO: Location Logic for Collected
            DrawCollected();
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

        #region Drawable Methods
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
            deckCards.Clear();
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


        protected virtual void board_MultipleMatch(object sender, MultipleMatchEventArgs args)
        {

        }

        #endregion


        #region Player EventHandler
        
        protected virtual void player_MouseOverCard(object sender, PlayerEventArgs args)
        {
            var mainP = (IMainPlayer)sender;
            if (mainP == null || mainP != MainPlayer)
                return;
            var toOutline = args.Card;
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

        }
        #endregion
        
        #region Location Methods

        private void PlaceCardOnHand(IHanafudaPlayer owner, DrawableCard drawable)
        {
            handCards[owner].Add(drawable);
            drawable.Position = GetHandLocation(owner);
        }

        /// Hardcoded for test screen
        public Vector2 GetHandLocation(IHanafudaPlayer owner)
        {
            int slot = handCards[owner].Count - 1;
            Vector2 position = new Vector2(50.0f * slot, 45.5f);
            position.X += 2 * slot + 30;
            if (owner == MainPlayer)
                position.Y += 390.0f;
            return position;
        }

        public void PlaceCardOnField(DrawableCard drawable)
        {
            Month month = drawable.Card.Month;
            fieldCards[month].Add(drawable);
            drawable.Position = GetFieldLocation(month);
        }

        public Vector2 GetFieldLocation(Month month)
        {
            int slot = (int)month;
            int xOffSet = (slot % 6);
            int yOffSet = (slot / 6) + 1;
            Vector2 position = new Vector2(80.0f * xOffSet, 100 * yOffSet);
            position.X += 30.0f + ((fieldCards[month].Count - 1) * 6) + (xOffSet -1) * 2;
            position.Y += 100;
            return position;
        }
        #endregion
    }
}
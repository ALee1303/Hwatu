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
        public HanafudaController Controller { get => ((IMainPlayer)MainPlayer).Controller; }
        
        public Sprite2D Outline { get => cardFactory.Outline; }

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
                if (Controller.IsMouseOverSelectable(selectable) &&
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
            if (CurrentPlayer is IMainPlayer)
                selectableCards = handCards[CurrentPlayer];
            // TODO input logic
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


        #region Player EventHandler Subscriber
        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
        }
        #endregion

        #region Drawable events

        public void drawable_MovedToDeck(DrawableCard drawable, HanafudaEventArgs args)
        {
            IHanafudaPlayer owner = args.Owner;
            bool revealed = args.Revealed;
            if (owner == MainPlayer)
            {
                if (collectedCards[owner].Remove(drawable))
                    handCards[owner].Remove(drawable);
            }
            else if (owner == null)
            {
                Month idx = drawable.Card.Month;
                fieldCards[idx].Remove(drawable);
            }
            else
            {
                if (revealed)
                    collectedCards[owner].Remove(drawable);
                else
                    handCards[owner].Remove(drawable);
            }
            deckCards.Add(drawable);
            drawable.Position = Vector2.Zero;
        }

        public void drawable_MovedToHand(DrawableCard drawable, HanafudaEventArgs args)
        {
            IHanafudaPlayer owner = args.Owner;
            bool revealed = args.Revealed;
            if (revealed)
            {
                Month idx = drawable.Card.Month;
                fieldCards[idx].Remove(drawable);
            }
            else
                deckCards.Remove(drawable);
            handCards[owner].Add(drawable);
            drawable.Position = GetHandLocation(owner);
        }

        public void drawable_MovedToField(DrawableCard drawable)
        {
            Month idx = drawable.Card.Month;
            deckCards.Remove(drawable);
            fieldCards[idx].Add(drawable);
            drawable.Position = GetFieldLocation(idx);
        }

        #endregion

        #region Location Methods

        private void PlaceCardOnHand(IHanafudaPlayer owner, DrawableCard card)
        {
            card.Position = GetHandLocation(owner);
            handCards[owner].Add(card);
        }

        /// Hardcoded for test screen
        public Vector2 GetHandLocation(IHanafudaPlayer owner)
        {
            int slot = handCards[owner].Count + 1;
            Vector2 position = new Vector2(60.0f * slot, 81.0f);
            position.Y -= 30;
            position.X -= (5) * slot;
            if (owner == MainPlayer)
                position.Y += 400;
            return position;
        }

        public void PlaceCardOnField(DrawableCard drawable)
        {
            Month month = drawable.Card.Month;
            drawable.Position = GetFieldLocation(month);
        }

        public Vector2 GetFieldLocation(Month month)
        {
            int slot = (int)month;
            int xOffSet = (slot % 6) + 1;
            int yOffSet = (slot / 6) + 1;
            Vector2 position = new Vector2(100.0f * xOffSet, 100 * yOffSet);
            position.X = position.X + ((fieldCards[month].Count - 1) * 10) - (5 * xOffSet);
            position.Y += 150;
            return position;
        }
        #endregion
    }
}
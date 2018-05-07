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

namespace GoStop.MonoGameComponents
{
    // TODO: separate into multiple types of board for GoStop and Minhwatu
    // Currently only supports Minhwatu 1p 1v1 with CPU
    public class BoardManager : GameComponent
    {
        private IBoard _board;
        private IHanafudaPlayer _mainPlayer;

        private CardFactory spriteFactory;
        private List<DrawableCard> deckCards;
        private Dictionary<Month, List<DrawableCard>> fieldCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> handCards;
        private Dictionary<IHanafudaPlayer, List<DrawableCard>> collectedCards;
        private List<DrawableCard> selectableCards;
        private DrawableCard selectedCard;

        public IHanafudaPlayer MainPlayer { get => _mainPlayer; }
        public IHanafudaPlayer CurrentPlayer { get => _board.CurrentPlayer; }
        public HanafudaController Controller { get => ((IMainPlayer)MainPlayer).Controller; }

        public Sprite2D BackImage { get => spriteFactory.BackImage; }
        public Sprite2D Outline { get => spriteFactory.Outline; }

        public BoardManager(Game game) : base(game)
        {
            Game.Services.AddService<BoardManager>(this);
            spriteFactory = new CardFactory(Game);
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
            _board = new MinhwatuBoard();
            ((Board)_board).AllPlayerRemoved += board_AllPlayerRemoved;
            ((Board)_board).NewPlayerTurn += board_NewPlayerTurn;
            ((Board)_board).MultipleMatch += board_MultipleMatch;
            if (_board.IsNewPlayer(MainPlayer))
                AddPlayerToBoard(MainPlayer);
            InitializeDrawables();
            // TODO: add CPU
        }

        /// <summary>
        /// Start game on board that is ready
        /// </summary>
        public void RestartLoadedBoard()
        {
            // TODO: Add case for detecting board's status and
            // Should not work when game is in progress or Drawables are not ready.
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
                _mainPlayer = player;
            if (_board != null)
                AddPlayerToBoard(player);
        }

        public void OnExitBoard(IHanafudaPlayer player)
        {
            // TODO: for multiple players
        }

        public override void Initialize()
        {
            // Currently not needed
            // being done when manually starting the game.
        }

        public override void Update(GameTime gameTime)
        {
            if (selectableCards.Count > 0)
            {
                OnCardSelectable();
            }
        }

        private void OnCardSelectable()
        {
            foreach (DrawableCard selectable in selectableCards)
            {
                if (Controller.IsMouseOverSelectable(selectable) &&
                    Controller.IsLeftMouseClicked())
                    OnCardSelected(selectable);
            }
        }

        #region Drawable Methods
        /// <summary>
        /// Reorder deck for new game
        /// </summary>
        /// TODO: sort without creating new Drawable
        private void ResetDrawables()
        {
            DiscardDrawables();
            InitializeDrawables();
        }

        /// <summary>
        /// Initialize drawables
        /// Called right before every game when all cards are in deck
        /// </summary>
        private void InitializeDrawables()
        {
            foreach (Hanafuda card in _board.Deck)
            {
                DrawableCard drawable = spriteFactory.ReturnPairedDrawable(card);
                drawable.Initialize();
                deckCards.Add(drawable);
            }
        }

        private void DiscardDrawables()
        {
            deckCards.Clear();
            fieldCards.Keys.ToList().ForEach(key => fieldCards[key].Clear());
            handCards.Keys.ToList().ForEach(key => handCards[key].Clear());
            collectedCards.Keys.ToList().ForEach(key => collectedCards[key].Clear());
        }
        #endregion

        #region Event Handler

        protected virtual void OnCardSelected(DrawableCard selected)
        {
            selectedCard = selected;
        }

        #endregion

        #region Handler Subscriber
        protected virtual void board_NewPlayerTurn()
        {
            if (CurrentPlayer is IMainPlayer)
                selectableCards = handCards[CurrentPlayer];
            // TODO input logic
        }

        protected virtual void board_AllPlayerRemoved()
        {
            ResetDrawables();
            // TODO: replay button
        }


        protected virtual void board_MultipleMatch(object sender, MultipleMatchEventArgs args)
        { }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
        }
        #endregion

        #region card moving
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

        /// Hardcoded for test screen
        public Vector2 GetHandLocation(IHanafudaPlayer owner)
        {
            int slot = handCards[owner].Count;
            Vector2 position = new Vector2(25.0f * slot, 41.0f * slot);
            position.Y += 10;
            position.X += 5 * slot;
            if (owner == MainPlayer)
                position.Y += 900;
            return position;
        }

        public Vector2 GetFieldLocation(Month month)
        {
            int slot = (int)month;
            int xOffSet = (slot % 6) + 1;
            int yOffSet = (slot / 6) + 1;
            Vector2 position = new Vector2(30.0f * xOffSet, 200 * yOffSet);
            return position;
        }
        #endregion
    }
}
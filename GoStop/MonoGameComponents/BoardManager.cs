using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

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
        private Dictionary<Month, DrawableCard> fieldCards;
        private List<DrawableCard> handCards;
        private List<DrawableCard> collectedCards;
        private List<DrawableCard> selectableCards;

        public IHanafudaPlayer MainPlayer { get => _mainPlayer; }
        public IHanafudaPlayer CurrentPlayer { get => _board.CurrentPlayer; }

        public BoardManager(Game game) : base(game)
        {
            Game.Services.AddService<BoardManager>(this);
            spriteFactory = new CardFactory(Game);
            deckCards = new List<DrawableCard>();
            fieldCards = new Dictionary<Month, DrawableCard>();
            handCards = new List<DrawableCard>();
            collectedCards = new List<DrawableCard>();
            selectableCards = new List<DrawableCard>();
        }
        
        public void StartMinhwatuGameVsCPU()
        {
            _board = new MinhwatuBoard();
            ((Board)_board).AllPlayerRemoved += board_AllPlayerRemoved;
            ((Board)_board).NewPlayerTurn += board_NewPlayerTurn;
            ((Board)_board).MultipleMatchFound += board_MultipleMatchesFound;
        }

        public void OnJoinBoard(IHanafudaPlayer player)
        {
            if (player is IMainPlayer)
                _mainPlayer = player;
            if (_board != null)
                _board.SubscribePlayer(player);
        }

        public void OnExitBoard(IHanafudaPlayer player)
        {
            // TODO: for multiple players
        }

        /// <summary>
        /// Called right before every game when all cards are in deck
        /// </summary>
        private void PairCardsWithImage()
        {
            foreach (Hanafuda card in _board.Deck)
                deckCards.Add(
                    spriteFactory.ReturnedPairedDrawable(card));
        }

        protected virtual void board_AllPlayerRemoved()
        {

        }

        protected virtual void board_NewPlayerTurn()
        {

        }

        protected virtual void board_MultipleMatchesFound(object sender, MatchEventArgs args)
        { }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            base.player_CardPlayed(sender, args);
        }
    }
}

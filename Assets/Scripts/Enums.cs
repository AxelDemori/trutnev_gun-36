public enum Team
{
    White,
    Black
}

public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}

public enum GameState
{
    WhiteTurn,
    BlackTurn,
    WhiteWin,
    BlackWin,
    Draw,
    Check,
    Checkmate
}

public enum MoveType
{
    Normal,
    Capture,
    Castling,
    EnPassant,
    Promotion
}
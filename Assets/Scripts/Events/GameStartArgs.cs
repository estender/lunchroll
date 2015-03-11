using System;

/// <summary>
/// Game start event arguments.
/// </summary>
public class GameStartArgs : EventArgs
{
	public DifficultySetting Difficulty { get; set; }
}


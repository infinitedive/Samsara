using System;
using System.Collections.Generic;

public class EventManager
{
    // Singleton instance
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
    }

    // Events
    public event Action<Player> OnPlayerJoined;
    public event Action<Player, int> OnPlayerScored;

    // List of players
    private List<Player> players = new List<Player>();

    // Method to add a player to the list and raise the OnPlayerJoined event
    public void AddPlayer(Player player)
    {
        players.Add(player);
        OnPlayerJoined?.Invoke(player);
    }

    // Method to notify other players when a player scores
    public void NotifyPlayerScore(Player scoringPlayer, int score)
    {
        foreach (Player player in players)
        {
            if (player != scoringPlayer) // Exclude the scoring player
                player.HandlePlayerScore(scoringPlayer, score);
        }
    }
}

public class Player
{
    public string Name { get; private set; }

    public Player(string name)
    {
        Name = name;
    }

    // Method to handle when another player joins
    public void HandlePlayerJoined(Player joinedPlayer)
    {
        Console.WriteLine($"{Name} received notification: {joinedPlayer.Name} joined the game.");
    }

    // Method to handle when another player scores
    public void HandlePlayerScore(Player scoringPlayer, int score)
    {
        Console.WriteLine($"{Name} received notification: {scoringPlayer.Name} scored {score} points.");
    }
}

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         // Create instances of players
//         Player player1 = new Player("Player 1");
//         Player player2 = new Player("Player 2");

//         // Subscribe players to event manager's events
//         EventManager.Instance.OnPlayerJoined += player1.HandlePlayerJoined;
//         EventManager.Instance.OnPlayerJoined += player2.HandlePlayerJoined;

//         // Add players to the event manager
//         EventManager.Instance.AddPlayer(player1);
//         EventManager.Instance.AddPlayer(player2);

//         // Simulate player scoring
//         EventManager.Instance.NotifyPlayerScore(player1, 10);
//     }
// }

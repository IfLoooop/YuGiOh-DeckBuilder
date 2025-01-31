﻿using System.Collections.Generic;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

/// <summary>
/// Represents one card in a <see cref="Pack"/>
/// </summary>
/// <param name="Endpoint">Endpoint for this card</param>
/// <param name="Rarities">Rarities for this card</param>
internal sealed record Card(string Endpoint, IEnumerable<Rarity> Rarities)
{
    #region Properties
    /// <summary>
    /// Endpoint for this card
    /// </summary>
    public string Endpoint { get; } = Endpoint;
    /// <summary>
    /// 8-digit unique id
    /// </summary>
    public int Passcode { get; set; } = -1;
    /// <summary>
    /// Rarities for this card
    /// </summary>
    [JsonIgnore]
    internal IEnumerable<Rarity> Rarities { get; set; } = Rarities;
    /// <summary>
    /// Indicates whether this card should be skipped during a card update
    /// </summary>
    [JsonIgnore]
    internal bool Skip { get; set; }
    /// <summary>
    /// Index of this card in <see cref="MainWindow.CardImages"/> 
    /// </summary>
    [JsonIgnore]
    internal int Index { get; set; }
    #endregion

    #region Methods
    public override string ToString()
    {
        return $"    \"{nameof(this.Endpoint)}\": \"{this.Endpoint}\",\n" +
               $"    \"{nameof(this.Passcode)}\": {this.Passcode}";
    }
    #endregion
}
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.Game
{
  public class ItemDescriptionUpdater : MonoBehaviour
  {
    [SerializeField] private List<TMP_Text> _descs;
    private CharacteristicConverter _charConverter;

    public void Construct(CharacteristicConverter charConverter)
    {
      _charConverter = charConverter;

      SetText("");
    }

    public void UpdateDescription(params Characteristic[] chars) =>
      SetText(chars.Aggregate("",
                              (current, characteristic) =>
                                current +
                                $"{_charConverter[characteristic.CharType]}: {Mathf.RoundToInt(characteristic.Value)}\n"));

    private void SetText(string text) => _descs.ForEach(x => x.text = text);
  }

  public struct Characteristic
  {
    public Type CharType;
    public float Value;
  }
}
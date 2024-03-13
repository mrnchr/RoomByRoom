using Configuration;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace RoomByRoom
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/InitializeData")]
  public class ProgressDataSO : ScriptableObject, ISelfBinder<ProgressDataSO>
  {
    [FormerlySerializedAs("Save")] public ProgressData Value;
    
    public void Bind(DiContainer container)
    {
      (this as ISelfBinder<ProgressDataSO>).BindSelf(container);
    }
  }
}
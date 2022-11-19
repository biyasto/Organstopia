using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Samples.InGameMailbox
{
    public class InventoryPopupView : MonoBehaviour
    {
        public async Task Show()
        {
            gameObject.SetActive(true);

            await EconomyManager.instance.RefreshInventory();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void OnCloseButtonPressed()
        {
            Close();
        }
    }
}

using System.Windows;
using CateringIS.Data;

namespace CateringIS
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            AppDatabase.Instance.Save();
            base.OnExit(e);
        }
    }
}

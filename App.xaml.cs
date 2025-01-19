using ProoiectVladSipos.Data;

namespace ProoiectVladSipos
{
    public partial class App : Application
    {
        private static CreditsDatabase _database;

        public static CreditsDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new CreditsDatabase(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Credits.db3")
                    );
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }

}

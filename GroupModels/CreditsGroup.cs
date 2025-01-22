using System.Collections.ObjectModel;
using ProoiectVladSipos.Models;

namespace ProoiectVladSipos.GroupModels
{
    public class CreditsGroup : ObservableCollection<Credits>
    {
        public string UserName { get; private set; }
        public int UserId { get; private set; }

        public CreditsGroup(User user, IEnumerable<Credits> credits)
            : base(credits)
        {
            UserName = user.Name;
            UserId = user.ID;
        }
    }
}

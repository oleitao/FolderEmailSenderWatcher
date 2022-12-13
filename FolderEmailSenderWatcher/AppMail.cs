namespace FolderEmailSenderWatcher
{
    public class AppMail
    {
        private int _id;
        private string _company;
        private string _manager;
        private string _folder;
        private string _email1;
        private string _email2;

        public AppMail(int id, string company, string manager, string folder, string email1, string email2)
        {
            _id = id;
            _company = company;
            _manager = manager;
            _folder = folder;
            _email1 = email1;
            _email2 = email2;
        }

        public int Id { get => _id; set => _id = value; }
        public string Company { get => _company; set => _company = value; }
        public string Manager { get => _manager; set => _manager = value; }
        public string Folder { get => _folder; set => _folder = value; }
        public string Email1 { get => _email1; set => _email1 = value; }
        public string Email2 { get => _email2; set => _email2 = value; }        
    }
}

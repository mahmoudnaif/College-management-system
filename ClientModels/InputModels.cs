namespace College_managemnt_system.ClientModels
{
 
        public class SearchModel
        {
            public string searchQuery { get; set; }
            public TakeSkipModel takeSkip { get; set; } = new TakeSkipModel();
        }

        public class Siqninmodel
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class SiqnupModel
        {

            public string email { get; set; }

            public string password { get; set; }

            public string repeatPassword { get; set; }

        }

        public class AdminChangeModel
        {

            public string email { get; set; }

            public string probertyChange { get; set; } = "";

        }


        public class ChangePasswordModel
        {
            public string oldPassword { get; set; }

            public string newPassword { get; set; } = "";
        }

        public class ChangePasswordEmailModel
        {
            public string newPassword { get; set; }

            public string repeatNewPassword { get; set; }
        }




      
        public class TakeSkipModel
        {
            public int take { get; set; } = 10;
            public int skip { get; set; } = 0;

        }

        public class EditDepartmentNameModel
        {
             public int departmentId { get; set; }
            public string newName { get; set; }
     }

      

    }

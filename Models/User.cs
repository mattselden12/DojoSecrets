using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DojoSecrets.Models
{
    public class User
    {
        public int UserId {get;set;}

        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string Email {get;set;}

        public string Password {get;set;}


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public List<Like> LikesByUser {get;set;}

        public List<Secret> SecretsByUser {get;set;}

        public User(){
            LikesByUser = new List<Like>();
            SecretsByUser = new List<Secret>();
        }
    }
}
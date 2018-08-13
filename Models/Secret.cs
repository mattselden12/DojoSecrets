using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DojoSecrets.Models
{
    public class Secret
    {
        public int SecretId {get;set;}

        public string Content {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int UserId {get;set;}

        public User Creator {get;set;}

        public List<Like> SecretLikes {get;set;}

        public Secret(){
            SecretLikes = new List<Like>();
        }
    }
}
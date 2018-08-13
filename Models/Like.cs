using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DojoSecrets.Models
{
    public class Like
    {
        public int LikeId {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int UserId {get;set;}

        public User CommentingUser {get;set;}

        public int SecretId {get;set;}

        public Secret SecretCommentedOn {get;set;}

    }
}
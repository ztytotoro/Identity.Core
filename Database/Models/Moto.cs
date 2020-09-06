using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Moto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime PublishTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdateTime { get; set; }
        public bool IsActive { get; set; }
        public Guid PublisherId { get; set; }
        public MotoUser Publisher { get; set; }
    }
}

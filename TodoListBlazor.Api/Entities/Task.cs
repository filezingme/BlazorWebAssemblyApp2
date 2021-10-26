﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Models.Enums;

namespace TodoListBlazor.Api.Entities
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public Guid? AssigneeId { get; set; }

        [ForeignKey("AssigneeId")]
        public User Assignee { get; set; }

        public DateTime CreatedDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}

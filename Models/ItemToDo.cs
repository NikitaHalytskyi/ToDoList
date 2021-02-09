using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ItemToDo
    {
        public int Id { get; set; }
        public string Text { get; set; }  //Текст задачи 
        public bool IsDone { get; set; } //Выполнена или же нет
    }
}

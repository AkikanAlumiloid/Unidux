﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Unidux.Example.Todo
{
    [Serializable]
    public class State : StateBase<State>
    {
        public TodoState Todo { get; set; }

        public State()
        {
            this.Todo = new TodoState();
        }
    }

    [Serializable]
    public class TodoState : StateElement<TodoState>
    {
        public uint Index = 0;
        public List<Todo> List = new List<Todo>();
        public VisibilityFilter Filter = VisibilityFilter.All;

        public List<Todo> ListByFilter
        {
            get
            {
                return this.List.Where(todo =>
                        this.Filter == VisibilityFilter.All ||
                        (this.Filter == VisibilityFilter.Active && !todo.Completed) ||
                        (this.Filter == VisibilityFilter.Completed && todo.Completed)
                    )
                    .ToList();
            }
        }
    }
}
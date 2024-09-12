using ReactiveUI;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using ToDoList.DataModel;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string ToDoListFileName = "todolist.json";
        private ViewModelBase _contentViewModel;

        public MainWindowViewModel()
        {
            var service = new ToDoListService();
            ToDoList = LoadToDoList() ?? new ToDoListViewModel(service.GetItems());
            _contentViewModel = ToDoList;
        }

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public ToDoListViewModel ToDoList { get; }

        public void AddItem()
        {
            AddItemViewModel addItemViewModel = new();

            Observable.Merge(
                addItemViewModel.OkCommand,
                addItemViewModel.CancelCommand.Select(_ => (ToDoItem?)null))
                .Take(1)
                .Subscribe(newItem =>
                {
                    if (newItem != null)
                    {
                        ToDoList.ListItems.Add(newItem);
                    }
                    ContentViewModel = ToDoList;
                });

            ContentViewModel = addItemViewModel;
        }

        public void SaveToDoList()
        {
            var items = ToDoList.ListItems;
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(ToDoListFileName, json);
        }

        private ToDoListViewModel LoadToDoList()
        {
            if (File.Exists(ToDoListFileName))
            {
                var json = File.ReadAllText(ToDoListFileName);
                var items = JsonSerializer.Deserialize<ToDoItem[]>(json);
                return new ToDoListViewModel(items);
            }
            return null;
        }
    }
}

using TaskTracker.Helpers;

MenuOptions option;

do
{
    option = MenuNavigator.ShowMainMenu();

    switch (option)
    {
        case MenuOptions.Add:
            MenuNavigator.CreateTask();
            break;
            
        case MenuOptions.GetAll:
            MenuNavigator.GetAllTasks();
            break;

        case MenuOptions.GetOne:
            MenuNavigator.GetTask();
            break;

        case MenuOptions.Update:
            MenuNavigator.UpdateTask();
            break;

        case MenuOptions.Delete:
            MenuNavigator.DeleteTask();
            break;
    }
} while (option != MenuOptions.Exit);
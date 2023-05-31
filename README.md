# FSCmd

FSCmd  is a compact command-line tool, created with inspiration from utilities like nircmd. 
Its primary function is to streamline dependencies within a range of software projects.

It has been designed with a modular structure, enabling the addition of independent modules over time to address evolving user needs and expand functionality.

# Functions

Current set of functions : 

```
Usage: FSCmd <tool> [tool options]
Available tools:
        FSCmd help [tool]
        FSCmd alert                  Displays an alert on the screen.
        FSCmd deleteTask             Deletes task from the task scheduler.
        FSCmd hibernate              Puts the computer into hibernation.
        FSCmd sleep                  Puts the computer into sleep.
        FSCmd setAlarm               Sets an alarm.
```

# Help export
FSCmd will incorporate a Help Export feature. This allows users to export the help texts for each command, offering potential for use as embeddings in AI applications (actually, this and dependency reduction is the main reason for this project)

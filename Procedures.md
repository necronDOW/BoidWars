*<- [Back to Repository](https://github.com/necronDOW/BoidWars)*

# Procedures
This document provides instructions for contributing to this project in addition to tips for avoid merge conflicts.

## Instructions
### Adding an Issue
1. Navigate to the [issues page](https://github.com/necronDOW/BoidWars/issues).
2. Click 'New Issue'.
3. Provide a title and description of the issue, do not provide an issue number as these are auto-assigned.
4. Assign appropriate labels (e.g. a feature would be labelled as 'enhancement').
5. **OPTIONAL**: If you intend on solving this issue yourself, click 'assign yourself' under "Assignees".

### Branching
1. Ensure that you are currently within the master branch. If you are not, type *"git checkout master"* before proceeding.
2. Type *"git checkout -b branch-name"*. The branch name should make use of '-' characters to seperate any spaces (e.g. "this is a branch" becomes "this-is-a-branch").


### Merging
1. Ensure that you are currently within the master branch. If you are not, type *"git checkout master"* before proceeding.
2. Type *"git merge branch-name"* to merge your branch, where "branch-name" is replaced by the name of your branch.
3. **OPTIONAL**: Delete your branch using *"git branch -d branch-name"*, where "branch-name" is replaced by the name of your branch.

**NOTE: If merge conflicts occur, consult the guide on resolving these conflicts [here](https://git-scm.com/book/en/v2/Git-Branching-Basic-Branching-and-Merging).**


### Adding a Feature
1. Firstly check the [issues list](https://github.com/necronDOW/BoidWars/issues) to see if the issue already exists. If the issue exists, proceed to step 2, otherwise see "Adding an issue" before proceeding to step 2.
2. Check that the issue does not already have any *assignees*. If the issue has assignees, contact the individual or find another issue, otherwise proceed to step 3.
3. Add yourself as an *assignee* and make note of the issue number (denoted as #[number]).
4. Branch from the master using the "Branching" instructions, ensure to reference the issue number and name as the branch name (e.g. "#45 add something").
5. Ensure that you are working within the new branch and proceed to make the changes you need to complete the issue.
6. Create a new scene named after the issue, seperating spaces using '-' characters, NEVER WORK IN THE MAIN SCENE.
7. If your fixed the issue, add and commit your changes using the following notation when naming your commit: "resolved #[issue number] [issue name]", e.g. "resolved #45 add something").
8. Merge your branch using the instructions under "Merging".


## Tips
### Avoiding Merge Conflicts

* Never work within the master branch.
* Never work on the main scene as an individual, this will avoid people ever working on the same scene.
* Duplicate the main scene whenever you create a new branch.

*<- [Back to Repository](https://github.com/necronDOW/BoidWars)*

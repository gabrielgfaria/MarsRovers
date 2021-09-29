# MarsRovers

## How To Run
You will find the executable located at the "/MarsRovers/bin/Release/net5.0".
Just double click it and you will see the output for the program on the prompt screen.

To customize the input just modify the file 'InputFile.txt' located at the root of the project with a valid input.

## Design and Assumptions

### Design
I designed the software in layers, each project has a different resonsability, as such:

- MarsRovers: Is the application layer, responsible for the dependency injection and input/output,
- Services: Holds the business logic, in this layer you will find the code that does what the project is meant to do,
- Models: Contains the entities that describe our system,
- UnitTests: Tests our service layer.

### Assumptions
- No rover can go out of bounds, what that means is that if the given plateau is of size (5, 5), no movement
can be made that would send the rover to position (6, 5), for exemple.
- A rover cannot be deployed to a position in which another rover currently resides.
- In its movements a rover cannot colide with another rover.

If any of the previously stated assumptions are to occur, an error will be displayed as output.


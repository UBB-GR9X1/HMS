![Image](https://github.com/user-attachments/assets/9d00bb43-c053-4a64-b6f2-0ab18cc226c2)

![image](https://github.com/user-attachments/assets/0503699d-3ce2-4b29-a75b-5bfb11c58eb7)

The migrations are somewhat working, tho a lot of the setup code is redundant, and I set most of them to not delete on cascade!
TODO: Make someone look into this cuz i'm too tired.
Atleast it works to create the database..
TODO: Seed the database

PS: The structure below changed a lot, so take a look in the models from Class Library (Shared) or make a diagram for the database.

To apply migrations: `dotnet ef database update` (assuming you have the necessary dependencies installed)

DATABASE STRUCTURE: (look into models)  
  
![image](https://github.com/user-attachments/assets/897c1ded-f2d3-4760-9f2c-cf16ac5b38f2)

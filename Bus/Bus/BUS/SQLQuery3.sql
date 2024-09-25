-- Create the Customer table
CREATE TABLE Customer (
    C_ID INT PRIMARY KEY IDENTITY(1,1),
    C_NAME VARCHAR(50) NOT NULL,
	PASSWORD VARCHAR(255) NOT NULL,
    EMAIL VARCHAR(100) UNIQUE NOT NULL,
    MOBILE_NUMBER VARCHAR(15),
    ADDRESS VARCHAR(255)
);
-- Create the Route table
CREATE TABLE Route (
    ROUTE_ID INT PRIMARY KEY IDENTITY(1,1),
    START_LOCATION VARCHAR(100) NOT NULL,
    END_LOCATION VARCHAR(100) NOT NULL,
    DISTANCE DECIMAL(10, 2) NOT NULL
);

-- Create the Bus table
CREATE TABLE Bus (
    BUS_ID INT PRIMARY KEY IDENTITY(1,1),
    BUS_NUMBER VARCHAR(20) NOT NULL,
    CAPACITY INT NOT NULL,
    ROUTE_ID INT,
);

-- Create the Schedule table
CREATE TABLE Schedule (
    SCHEDULE_ID INT PRIMARY KEY IDENTITY(1,1),
    BUS_ID INT,
    ROUTE_ID INT,
    DEPARTURE_TIME DATETIME NOT NULL,
    ARRIVAL_TIME DATETIME NOT NULL,
);

-- Create the Ticket table
CREATE TABLE Ticket (
    TICKET_ID INT PRIMARY KEY IDENTITY(1,1),
    C_ID INT,
    BUS_ID INT,
    SEAT_NUMBER VARCHAR(10) NOT NULL,
    JOURNEY_DATE DATE NOT NULL,
    PRICE DECIMAL(10, 2) NOT NULL
);

-- Create the Payment table
CREATE TABLE Payment (
    PAYMENT_ID INT PRIMARY KEY IDENTITY(1,1),
    TICKET_ID INT,
    PAYMENT_DATE DATETIME NOT NULL,
    PAYMENT_METHOD VARCHAR(50) NOT NULL,
    AMOUNT DECIMAL(10, 2) NOT NULL,
);

-- Create the relationship tables if needed

-- CHOOSE relationship table if many-to-many between Customer and Bus
CREATE TABLE Choose (
    C_ID INT,
    BUS_ID INT,
    PRIMARY KEY (C_ID, BUS_ID)
);

-- Insert sample data if necessary
-- (Examples)
INSERT INTO Customer (C_NAME, EMAIL, MOBILE_NUMBER, ADDRESS) VALUES 
('John Doe','john.doe@example.com', '123-456-7890', '123 Elm St, Anytown, CA'),
('Jane Smith', 'jane.smith@example.com', '987-654-3210', '456 Oak St, Othertown, TX');

INSERT INTO Route (START_LOCATION, END_LOCATION, DISTANCE) VALUES 
('City A', 'City B', 100.00),
('City B', 'City C', 150.50);

INSERT INTO Bus (BUS_NUMBER, CAPACITY, ROUTE_ID) VALUES 
('BUS1001', 50, 1),
('BUS1002', 40, 2);

INSERT INTO Schedule (BUS_ID, ROUTE_ID, DEPARTURE_TIME, ARRIVAL_TIME) VALUES 
(1, 1, '2024-07-01 08:00:00', '2024-07-01 12:00:00'),
(2, 2, '2024-07-01 09:00:00', '2024-07-01 13:00:00');

INSERT INTO Ticket (C_ID, BUS_ID, SEAT_NUMBER, JOURNEY_DATE, PRICE) VALUES 
(1, 1, 'A1', '2024-07-01', 50.00),
(2, 2, 'B1', '2024-07-01', 60.00);

INSERT INTO Payment (TICKET_ID, PAYMENT_DATE, PAYMENT_METHOD, AMOUNT) VALUES 
(1, '2024-06-30 10:00:00', 'Credit Card', 50.00),
(2, '2024-06-30 11:00:00', 'Debit Card', 60.00);
  

  Select * From Customer
    Select * From Route
      Select * From Bus
        Select * From Schedule
          Select * From Ticket
            Select * From Payment
              Select * From Choose
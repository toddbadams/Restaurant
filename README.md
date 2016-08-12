# Restaurant Order Entry System Example

This example has been design as a MVP of code style and contains a few common techniques and patterns used on modern internet facing enterprise level applications.  It is not intended as the only solution to a given problem, nor is it a production ready solution.  There remain a number of items to improve the solutions, such as implementing IOC, reduce code in the application services through attribute filters, however time is limited...

## Requirements
* SQL Server
* .Net 4.6.2
* Visual Studio 2013
* Postman

## How to run
1.  Download and build
2.  Import the Postman.json into Postman
3.  Create C:\logs
3.  Start
5.  Run the Postman collection


## Restaurant API
The following is an MSON representation of the Restaurant API which runs at localhost:9001

### Create a menu [POST]
This creates a restaurant's menu.

This action returns a 201 status code along with a JSON body.

+ Request (application/json)

       {
		  "name": "Todds",
		  "items": [
		    {
		      "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
		      "price": 19
		    },
		    {
		      "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
		      "price": 25
		    }
		  ]
		}

+ Response 201 (application/json)

    + Body

			{
			  "menuId": 1,
			  "name": "Todds",
			  "items": [
			    {
			      "id": 1,
			      "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
			      "price": 19
			    },
			    {
			      "id": 2,
			      "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
			      "price": 25
			    }
			  ]
			}

### Create a New Order [POST]
This creates a restaurant order, and attaches a menu from which items can be ordered.

This action returns a 201 status code along with a JSON body.

+ Request (application/json)

        {
		  "name": "Todd B Adams",
		  "menuId": 1
		}

+ Response 201 (application/json)

    + Body

            {
			  "orderId": 1,
			  "name": "Todd B Adams",
			  "items": [],
			  "menu": {
			    "menuId": 1,
			    "name": "Dinner Menu",
			    "items": [
			      {
			        "id": 1,
			        "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
			        "price": 19
			      },
			      {
			        "id": 2,
			        "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
			        "price": 25
			      }
			    ]
			  }
			}

### Get an Order [GET]
Gets the restaurant order by id.

This action returns a 200 status code along with a JSON body.

+ Response 200 (application/json)

            {
			  "orderId": 1,
			  "name": "Todd B Adams",
			  "items": [],
			  "menu": {
			    "menuId": 1,
			    "name": "Dinner Menu",
			    "items": [
			      {
			        "id": 1,
			        "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
			        "price": 19
			      },
			      {
			        "id": 2,
			        "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
			        "price": 25
			      }
			    ]
			  }
			}



### Add Items to the Order [POST]
This adds menu items to the diner.
This action returns a 200 status code along with a JSON body.

+ Request (application/json)

        [
		  {
		    "menuItemId": 1,
		    "quantity": 1
		  },
		  {
		    "menuItemId": 3,
		    "quantity": 1
		  },
		  {
		    "menuItemId": 10,
		    "quantity": 2
		  }
		]

+ Response 201 (application/json)

    + Body

{
  "dinerId": 1,
  "name": "Todd B Adams",
  "items": [
    {
      "menuItemId": 1,
      "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
      "price": 19,
      "quantity": 1
    },
    {
      "menuItemId": 2,
      "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
      "price": 25,
      "quantity": 1
    },

  ],
  "menu": {
    "menuId": 1,
    "name": "Dinner Menu",
    "items": [
      {
        "id": 1,
        "name": "Lentil crusted plaice fillet with pumpkin seed chutney, spiced yellow lentil and bottle gourd",
        "price": 19
      },
      {
        "id": 2,
        "name": "Spice crusted halibut with tomato tamarind sauce, ginger jaggery pickle",
        "price": 25
      }
    ]
  }
}



# Daily menu loader
The daily menu is a CSV parser that runs as a windows process.  It detects when files are added to the CSV directory (C:\csvfolder), and does the following:

* If the file is greater than a given size (expressed as the number of lines) it breaks it into smaller files of the maximum line size).
* For files with less than or equal to the maximum line size, each line is converted to a database entity which is then added to the database.

Currently this is logging the parsed menu.  The next step is to POST the menu into the restaurant api.


## How to run
2.  Create C:\csvfolder
3.  Create C:\logs
3.  Start CsvService
4.  Drop the Todds.csv file into C:\csvfolder (this will consume the menu file and delete it on completion)
5.  View the log to see consumed data


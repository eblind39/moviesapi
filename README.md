# Movie Challenge

## Environment:

-   **.NET Core 3.1**

-   In Memory Repositories

## Notes:

-   The Application **must** accept request on port **`8000`**, by the
    default is running over the port `8080` and must be changed.

-   The below resources are **read-only** and **must not been changed or
    push to the repository**:

    -   bin/\*

    -   api-challenge.postman\_collection.json

    -   dotnet.postman\_environment.json

## Requirements

Create the endpoints for a basic Movie Store management system. The
definitions and detailed requirements have been listed below.

### Movies CMS

Each `movie` data is a JSON object with following properties:

-   `movieId`: unique integer identifier

-   `title`: name of the movie

-   `description`: information about movie

-   `stock`: how many items are available for sale or rent

-   `rentalPrice`: how much it will cost to rent this movie

-   `salePrice`: how much it will cost to buy this movie

-   `available`: indicator that shows if the movie could be rented
    or/and sold

You need to implement the `/movies` REST endpoint for the following
operations:

`POST` requests to `/movies`:

-   Should store the movie into the repository and return the inserted
    information with status code 201

-   Title, stock, rental and sales price are required

-   Availability by default should be `true`

**Curl Request**

``` bash
curl --request POST \
    --url https://localhost/movies \
    --header 'Accept: application/json' \
    --data '{
        "title": "Amazing Spiderman",
        "description": "Another Spiderman Movie",
        "stock": 10,
        "rentalPrice": 25,
        "salePrice": 90,
        "available": true
    }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "title": "Amazing Spiderman",
  "description": "Another Spiderman Movie",
  "stock": 10,
  "rentalPrice": 25,
  "salePrice": 90,
  "available": true
}
```

`PUT` request to `/movies/{movieId}`

-   Should update the movie based on the given id and payload and
    response with status code 200

-   Title, stock, rental and sales price, are required

-   Availability by default should be `true`

**Curl Request**

``` bash
curl --request PUT \
  --url https://localhost/movies/1 \
  --header 'Accept: application/json' \
  --data '{
    "title": "Amazing Spiderman",
    "description": "Another Spiderman Movie",
    "stock": 10,
    "rentalPrice": 25,
    "salePrice": 90,
    "available": true
  }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "title": "Amazing Spiderman",
  "description": "Another Spiderman Movie",
  "stock": 10,
  "rentalPrice": 25,
  "salePrice": 90,
  "available": true
}
```

`PATCH` request to `/movies/{movieId}`

-   Should update the movie based on the given id and payload and return
    status code 200

**Curl Request**

``` bash
curl --request PATCH \
  --url https://localhost/movies/1 \
  --header 'Accept: application/json' \
  --data '{
    "title": "Amazing Spiderman",
    "description": "Another Spiderman Movie",
    "stock": 10,
    "rentalPrice": 25,
    "salePrice": 90,
    "available": true
  }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "title": "Amazing Spiderman",
  "description": "Another Spiderman Movie",
  "stock": 10,
  "rentalPrice": 25,
  "salePrice": 90,
  "available": true
}
```

`DELETE` request to `/movies/{movieId}`

-   Should delete the movie based on the given id

-   A cascade deletion should be performed

**Curl Request**

``` bash
curl --request DELETE \
  --url https://localhost/movies/1 \
  --header 'Accept: application/json'
```

**Http Body**

``` html
HTTP/1.1 200 OK
```

`GET` requests to `/movies`:

-   Should response a collection of movies. By default, only available
    movies should be response, unless `unavailable` parameter indicates
    otherwise. `unavailable=true` returns all the movies.

-   Should response a movie’s page by paginating with `size` and `page`
    parameters, and sorting with `sort` parameters (sort parameter
    syntax `sort=<field[,asc|,desc]>`). By default, each page must have
    at most 12 elements and start at the first page and sorted by a
    title ascendant. `page=1`, `size=20`, `sort=description,asc`

-   Should filter the movies by a `title` parameters. Where parameter is
    case-insensitive and could be just a peace of the movie’s title .

-   Should return an object containing the `content` property, that
    contains the array of Movies that have been found

-   Should return an object with the following properties:

    -   `size`: the size of the page requested

    -   `numberOfElements`: the number of elements that are found in the
        current page

    -   `totalElements`: the number of elements found with the given
        criteria

    -   `totalPages`: the number of pages available

    -   `number`: the current page number

**Curl Request**

``` bash
curl --request GET \
  --url https://localhost/movies \
  --header 'Accept: application/json'
```

**Response Body**
include::../snippets/movies.adoc\[tags=movies.get.all.response\]

### Sales

Each `sale` data is a JSON object with following properties:

-   `id`: unique integer identifier

-   `movieId`: unique integer identifier of the movie sold

-   `customerEmail`: customer email that bought the movie

-   `price`: the price witch the movie has been sold

You need to implement the `/sales` REST endpoint for the following
operations:

`POST` requests to `/sales`:

-   Should response 201 when the movie has been sold.

-   Only available movies could be sold.

-   Stock should be updated.

**Curl Request**

``` bash
curl --request POST \
  --url https://localhost/sales \
  --header 'Accept: application/json' \
  --data '{
    "movieId": 1,
    "customerEmail": "customer@domain.com"
  }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "customerEmail": "customer@domain.com",
  "price": 20
}
```

### Rentals

Each `rental` data is a JSON object with following properties:

-   `id`: unique integer identifier

-   `movieId`: unique integer identifier of the movie rented

-   `customerEmail`: customer email that rented the movie

-   `price`: the price witch the movie has been rented

You need to implement the `/rentals` REST endpoint for the following
operations:

`POST` requests to `/rentals`:

-   Should response 200 when the movie has been rented.

-   Only available movies could be rented.

-   Stock should be updated.

**Curl Request**

``` bash
curl --request POST \
  --url https://localhost/rentals \
  --header 'Accept: application/json' \
  --data '{
    "movieId": 1,
    "customerEmail": "customer@domain.com"
  }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "customerEmail": "customer@domain.com",
  "price": 15
}
```

### Likes

Each `like` data is a JSON object with following properties:

-   `movieId`: unique integer identifier of the movie sold

-   `likes`: total likes given by the customers

-   `customers`: arrays of the distinct customer that have liked the
    movie

You need to implement the `/likes` REST endpoint for the following
operations:

`POST` requests to `/likes`:

-   Should response 201 when the movie has been liked by a customer.

-   Only available movies could be liked.

**Curl Request**

``` bash
curl --request POST \
  --url https://localhost/likes \
  --header 'Accept: application/json' \
  --data '{
    "movieId": 1,
    "customerEmail": "customer@domain.com"
  }'
```

**Response Body**

``` json
{
  "movieId": 1,
  "likes": 1,
  "customers": ["customer@domain.com"]
}
```

### Transactions

Each `transaction` data is a JSON object with following properties:

-   `movieId`: unique integer identifier of the movie sold

-   `likes`: total likes given by the customers

-   `customers`: arrays of the distinct customer that have liked the
    movie

You need to implement the `/transactions` REST endpoint for the
following operations:

`GET` requests to `/transactions/movies/{movieId}`:

-   Should return information of the movie’s transaction bases on the
    given movie id.

-   Should return 404 when information has not been found.

-   Should return the information base on a range of dates, whether any
    of the rental or sale transaction has been made (both inclusive).
    `from=2000-01-01` `to=2021-12-31`

**Curl Request**

``` bash
curl --request GET \
  --url https://localhost/transactions/movies/1?from=2000-01-01&to=2021-12-31 \
  --header 'Accept: application/json'
```

**Response Body**

``` json
{
  "movieId": 1,
  "rentals": [
    "2021-01-09",
    "2020-12-09"
  ],
  "sales": [
    "2019-09-19",
    "2012-02-24"
  ],
  "totalRevenue": 89,
  "customers": [
    "ssanchez@gmail.com",
    "peter@yahoo.com"
  ]
}
```

### Test Endpoints

A Postman Collection has been provided for you, this collection has the
following folder:

1.  `start-here`: Here are some request examples that you can run
    against your application. We suggest to start developing these
    endpoints first, since they are the ones required for every test in
    the collection.

2.  `tests`: Here you can find the request along with the test
    scenarios, the scenarios have been design based on the requirements,
    and will be used to grade your solution.

You can follow the steps to [**import a collection into
Postman**](https://learning.postman.com/docs/getting-started/importing-and-exporting-data/)

Here are some useful information on [**how to run a
collection**](https://learning.postman.com/docs/running-collections/intro-to-collection-runs/).
Keep in mind that you will need to select the environment for all the
test to run.

You will need to import the below files (***The files could be found at
the root level of the project***):

1.  `api-challenge.post_collection.json`: Postman Collection.

2.  `dotnet.postman_environment.json`: Environment variables needed by
    the collection

### Run Commands

If you want to run or install the application, here are some useful
commands:

**Start the application**

``` bash
dotnet run --project Challenge.API
```

**Install the application**

``` bash
dotnet build Challenge.API
```

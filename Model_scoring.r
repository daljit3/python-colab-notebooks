library("RCurl")
library("rjson")

# Accept SSL certificates issued by public Certificate Authorities
options(RCurlOptions = list(cainfo = system.file("CurlSSL", "cacert.pem", package = "RCurl"), ssl.verifypeer = FALSE))

h = basicTextGatherer()
hdr = basicHeaderGatherer()

# Request data goes here
req = list(
    Inputs = list(
        "WebServiceInput0" = list(
            list(
                'symboling' = "3",
                'normalized-losses' = "1",
                'make' = "alfa-romero",
                'fuel-type' = "gas",
                'aspiration' = "std",
                'num-of-doors' = "two",
                'body-style' = "convertible",
                'drive-wheels' = "rwd",
                'engine-location' = "front",
                'wheel-base' = "88.6",
                'length' = "168.8",
                'width' = "64.1",
                'height' = "48.8",
                'curb-weight' = "2548",
                'engine-type' = "dohc",
                'num-of-cylinders' = "four",
                'engine-size' = "130",
                'fuel-system' = "mpfi",
                'bore' = "3.47",
                'stroke' = "2.68",
                'compression-ratio' = "9",
                'horsepower' = "111",
                'peak-rpm' = "5000",
                'city-mpg' = "21",
                'highway-mpg' = "27",
                'price' = "13495"
            )
        )
    ),
    GlobalParameters = setNames(fromJSON('{}'), character(0))
)

body = enc2utf8(toJSON(req))
api_key = "mBD2JVfNf1UD7QS7gLwEHHADg2bQSRbP" # Replace this with the API key for the web service
authz_hdr = paste('Bearer', api_key, sep=' ')

h$reset()
curlPerform(
    url = "http://cea90c4e-8de0-4d85-b2c5-ea181497591c.eastus.azurecontainer.io/score",
    httpheader=c('Content-Type' = "application/json", 'Authorization' = authz_hdr),
    postfields=body,
    writefunction = h$update,
    headerfunction = hdr$update,
    verbose = TRUE
)

headers = hdr$value()
httpStatus = headers["status"]
if (httpStatus >= 400)
{
    print(paste("The request failed with status code:", httpStatus, sep=" "))

    # Print the headers - they include the request ID and the timestamp, which are useful for debugging the failure
    print(headers)
}

print("Result:")
result = h$value()
print(fromJSON(result))

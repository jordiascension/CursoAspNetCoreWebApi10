-Para ejecutar las migraciones  
    1.Tools-->CommandLine-->Terminal  
    2.cd School  
    3.dotnet ef migrations add Factura  
    4.dotnet ef database update  


-Postman:  

    1. Post:  https://localhost:7245/api/invoice  

    {
        "date": "2026-03-04T00:00:00",
        "lines": [
            {
            "description": "Laptop",
            "quantity": 1,
            "unitPrice": 1200,
            "vatRate": 21
            },
            {
            "description": "Mouse",
            "quantity": 2,
            "unitPrice": 25,
            "vatRate": 21
            }
        ]
    }
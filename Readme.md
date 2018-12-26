[![Build Status](https://travis-ci.org/MaxGsomGsom/CreditCalc.svg?branch=master)](https://travis-ci.org/MaxGsomGsom/CreditCalc)

## API description
Route: ``/api/credit/payments``

Parameters:
 - amount
 - interest (format: ``0.077`` or ``7.7%``)
 - downpayment (optional)
 - term

Response in JSON format:
```
{
  "monthly payment": 0,
  "total interest": 0,
  "total payment": 0
}
```
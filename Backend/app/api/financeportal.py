#~/CW/app/api/services/financeportal.py
#Finance Portal
from fastapi import FastAPI
from typing import List
from fastapi import Header, APIRouter, HTTPException
from fastapi.responses import HTMLResponse
from fastapi.security import HTTPBasic



finance = APIRouter()

#Finance microservice which opens a "portal" to an external financing site
@finance.get('/Finance/Portal', response_class=HTMLResponse)
async def index():
 return """
<html>
    <head>
        <title>Enable Financing</title>
    </head>
    <body>
        <h1> Enable Financing Portal </h1>
    </body>
</html>
"""










#Services and main script
#Uvicorn required for hosting, in order to start app, navigate to main.py directory and use python3 -m uvicorn main:app --reload
#Will host to localhost by default on port 8000 (http://127.0.0.1:8000/)
#Required libraries: FastAPI, Uvicorn, Pydantic
#pip3 install FastAPI, Uvicorn, Pydantic --user
#uses Python 3.10.11 64bit



#~/CW/app/main.py
from fastapi import FastAPI
from .api.items import items
from .api.stock import stock
from .api.loyalty import offers
from .api.financeportal import finance

app = FastAPI()

#Include all of the services in the API Router
app.include_router(offers)
app.include_router(items)
app.include_router(stock)
app.include_router(finance)
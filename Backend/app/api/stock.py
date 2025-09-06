#~/CW/app/api/services/stock.py
#Stock Control microservice
from fastapi import FastAPI
from typing import List
from fastapi import Header, APIRouter, HTTPException

from ..models.item_stock import Stock


#Fake database of dictionaries for the sake of simplicity
fake_item_db = [
    {
        "name" : "Bread",
        "stock" : 23
    },
    {
        "name" : "Milk",
        "stock" : 321
    },
    {
        "name" : "Biscuits",
        "stock" : 928
    },
    {
        "name" : "Peanuts",
        "stock" : 133
    },
    {
        "name" : "PlasticBag",
        "stock" : 538
    },
    {
        "name" : "Thing",
        "stock": 3
    },
    {
        "name" : "AABatteries",
        "stock" : 142
    }
]





stock = APIRouter()



#Default endpoint which lists all of the items - for debugging purposes
@stock.get('/Stock/ListAll')
async def index():
    return fake_item_db

#Endpoint which returns items that are low on stock (<100), used by the client to list items that may require orders
@stock.get('/Stock/ListLow')
async def index():
    low_stock_db = []
    for stockItem in fake_item_db:
        if stockItem['stock'] < 100:
            low_stock_db.append(stockItem)
    return low_stock_db
#CRUD
@stock.post('/Stock/Add', status_code=201)
async def add_stock(payload: Stock):
    item = payload.dict()
    fake_item_db.append(item)
    return {'id': len(fake_item_db) - 1}
#Update endpoint which receives a list of objects from the client and will update the dictionaries based on what items were ordered
@stock.put('/Stock/Update')
async def update_stock(payload: List[Stock]):
    for stock_item in payload:
        for db_item in fake_item_db:
            if db_item["name"] == stock_item.name:
                db_item["stock"] = stock_item.stock
    return fake_item_db
   # raise HTTPException(status_code=404, detail="Item with given id not found")
#CRUD
@stock.delete('/Stock/Delete/{id}')
async def delete_stock(id: int):
    items_length = len(fake_item_db)
    if 0 <= id <= items_length:
        del fake_item_db[id]
        return None
    raise HTTPException(status_code=404, detail="Item with given id not found")
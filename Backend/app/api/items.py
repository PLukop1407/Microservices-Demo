# /app/api/items.py
#Price Control microservice
from fastapi import FastAPI
from typing import List
from fastapi import Header, APIRouter, HTTPException

from ..models.item_stock import Item


#Fake database of dictionaries for the sake of simplicity
fake_item_db = [
    {
        "id" : 1,
        "name" : "Bread",
        "price" : 1.99,
        "itemSale" : "HalfOff"
    },
    {
        "id" : 2,
        "name" : "Milk",
        "price" : 1.79,
        "itemSale" : "TwoForOne"
    },
    {
        "id" : 3,
        "name" : "Biscuits",
        "price" : 2.99,
        "itemSale" : "None"
    },
    {
        "id" : 4,
        "name" : "Peanuts",
        "price": 5.99,
        "itemSale" : "TwoForOne"
    },
    {
        "id" : 5,
        "name" : "PlasticBag",
        "price": 0.49,
        "itemSale" : "TwoForOne"
    },
    {
        "id" : 6,
        "name" : "Thing",
        "price": 99.99,
        "itemSale" : "None"
    },
    {
        "id": 7,
        "name" : "AABatteries",
        "price": 1.99,
        "itemSale" : "HalfOff"
    }
]



items = APIRouter()


#Endpoint which just returns the list of shop items to display on the client
@items.get('/Item/List')
async def index():
    return fake_item_db
#CRUD endpoint - not used by client, just for testing.
@items.post('/Item/Add', status_code=201)
async def add_item(payload: Item):
    item = payload.dict()
    fake_item_db.append(item)
    return {'id': len(fake_item_db) - 1}

#Update endpoint which allows prices and/or sales to be updated by the client
@items.put('/Item/Update/{id}')
async def update_item(id: int, payload: Item):
    item = payload.dict()
    items_length = len(fake_item_db)
    if 0 <= payload.id <= items_length:
        fake_item_db[payload.id-1] = item
        return None
    raise HTTPException(status_code=404, detail="Item with given id not found")

#CRUD endpoint - not used by the client, allows entries to be deleted.
@items.delete('/Item/Delete/{id}')
async def delete_item(id: int):
    items_length = len(fake_item_db)
    if 0 <= id <= items_length:
        del fake_item_db[id]
        return None
    raise HTTPException(status_code=404, detail="Item with given id not found")
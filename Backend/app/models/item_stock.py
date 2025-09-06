#~/CW/app/api/model/models.py
from typing import List
from pydantic import BaseModel



#Models for Price and Inventory Control microservices
class Item(BaseModel):
    id: int
    name: str
    price: float
    itemSale: str


class Stock(BaseModel):
    name: str
    stock: int

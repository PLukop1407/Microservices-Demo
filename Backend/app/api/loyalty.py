#~/CW/app/api/services/loyalty.py
#Loyalty microservice
from fastapi import FastAPI
from typing import List
from fastapi import Header, APIRouter, HTTPException

from ..models.item_stock import Item



fake_item_db = ["You have an offer for 50% off on Dairy products!", "You have an offer for 10% off on biscuits!", "You have 5% off on Raw Iron Ore"]

offers = APIRouter()

#Offer list endpoint for customer, returns the list of offers they have
@offers.get('/Offers/List')
async def index():
    return fake_item_db


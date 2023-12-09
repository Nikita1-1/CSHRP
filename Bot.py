import os
from bot_token import BOT_Token
import telebot
from pprint import pprint
from telegram import Update
import robin_stocks as rs


bot = telebot.TeleBot(BOT_Token)

@bot.message_handler(commands=['start'])
def login_rs(message):
    sent_msg = bot.send_message(message.chat.id, 'Welcome! Loggin in into your Robinhood account, wait...')
    auth_status = True
    if auth_status:
        rs.login(username='',
                 password='',
                 expiresIn=86400,
                 by_sms=True)
        bot.send_message(message.chat.id, "Logged in successfully!")
        bot.register_next_step_handler(send_welcome)
    else:
        bot.send_message(message.chat.id, "Authentication failed. Please try again.")
        bot.register_next_step_handler(login_rs)

def send_welcome(message):
    text =  "Do you want to *buy* or *sell* stocks today? Enter the command"
    sent_msg = bot.send_message(message.chat.id, text, parse_mode="Markdown")
    bot.register_next_step_handler(sent_msg, action_stock)


def action_stock(message):
    if message.text.lower() == 'buy':
        sent_msg = bot.send_message(message.chat.id, "Enter a list of stock names:")
        bot.register_next_step_handler(sent_msg, process_stock_list)
    elif message.text.lower() == 'sell':
        sent_msg = bot.send_message(message.chat.id, "Let's sell all this crap")
        bot.register_next_step_handler(sent_msg, sell_stocks)

def process_stock_list(message):
    user_input = message.text
    if user_input == '' or user_input.isdigit():
        bot.send_message(message.chat.id, "Please enter valid text, not a number.")
        bot.register_next_step_handler(message, send_welcome)
    else:
        stock_names = [stock.strip() for stock in user_input.split(',')]
        sent_msg = bot.send_message(message.chat.id, "Enter price for stocks:")
        bot.register_next_step_handler(sent_msg, lambda msg: get_price(msg, stock_names))

def get_price(message, stock_names):
    price = float(message.text)
    for stock in stock_names:
        rs.orders.order_buy_fractional_by_price(stock=stock,
                                                price=price,
                                                timeInForce='gtc',
                                                extendedHours=False)
    bot.send_message(message.chat.id, "All stocks have been processed!")


@bot.message_handler(commands=['cancel'])
def cancel_order(message,stock_names):
    rs.orders.cancel_all_stock_orders(stock_names)
    bot.send_message(message.chat.id, "All the stocks cancelled successfully!")
    bot.register_next_step_handler(send_welcome)


@bot.message_handler(commands=['sell'])
def sell_stocks(message, stock_names):
    for stock in stock_names:
        rs.orders.order_sell_market(stock)
    bot.send_message(message.chat.id, "All stocks sold successfully!")





bot.polling()
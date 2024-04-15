const { Telegraf, Markup} = require('telegraf');
const axios = require('axios');
const token = '7114363165:AAEc134MUkMTCvSec0m8Rhtfe1IhEryEgOM'
const bot = new Telegraf(token);
const switchAPIURL = 'http://178.208.76.115:5024/switch'


bot.command('start', (ctx) => ctx.reply("Привет!", commandsMenu));

//keyboard menu
const commandsMenu = Markup.inlineKeyboard([
    Markup.button.callback("Получить состояние", 'get'),
    Markup.button.callback("Установить состояние", 'set'),
]).persistent().resize();

const setMenu = Markup.inlineKeyboard([
    Markup.button.callback('true', 'setTrue'),
    Markup.button.callback('false', 'setFalse'),
])

const exitMenu = Markup.inlineKeyboard([
    Markup.button.callback('Выход', 'start')
])

//actions
bot.action('get', async (ctx) => {
    let response = await GET();
    ctx.editMessageText('Состояние: ' + response.data.state, exitMenu)
})

bot.action('start', (ctx) => (ctx.editMessageText("Привет!", commandsMenu)))

bot.action('set', (ctx) => {
    ctx.editMessageText('Выберете состояние', setMenu)
})

bot.action('setTrue', async (ctx) => {
    let response = await POST('true');
    ctx.editMessageText(await Status(response), exitMenu);
})

bot.action('setFalse', async (ctx) => {
    let response = await POST('false');
    ctx.editMessageText(await Status(response), exitMenu);
})

const Status = async(response) => {
    switch (response.status) {
        case 400:
            let get = await GET();
            return 'Состояние уже ' + get.data.state;
        case 200:
            return 'Состояние изменено';
        default:
            return 'Ошибка';
    }
}

const GET = async () =>{
    return await axios.get(switchAPIURL)
        .then((response) => {
            console.log(response.status);
            return response;
        })
        .catch((err) => {
            console.log(err.response.statusText);
        });
}

const POST = async (value) => {
    return await axios.post(switchAPIURL, {
            id: 1, 
            state: value
        })
        .then((response) => {
            console.log(response.statusText);
            return response;
        })
        .catch((err) => {
            console.log(err.response.status);
            return err.response;
        })
}

bot.launch().then(()=> console.log('launched'));

// Enable graceful stop
 process.once('SIGINT', () => bot.stop('SIGINT'));
 process.once('SIGTERM', () => bot.stop('SIGTERM'));
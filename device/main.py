
import hid
import time
import asyncio
from aiohttp import web


VENDOR_ID = 1351
PRODUCT_ID = 12291
HTML_FILE = 'index.html'

sockets = []

async def forward_inputs():

    def open_device(): 
        try:
            print("Opening device...")
            device = hid.device()
            device.open(VENDOR_ID, PRODUCT_ID)  # TREZOR VendorID/ProductID
            print("Device ok")
            return device
        except Exception as ex:
            print("HID error:", ex)
            print("Retrying in 5s...")
            time.sleep(5)
            return open_device()
        
    device = open_device()
    loop = asyncio.get_event_loop()

    while True:
        result = await loop.run_in_executor(None, lambda : str(device.read(64)))
        for ws in sockets:
            try:
                await ws.send_str(result)
            except ConnectionResetError:
                print("Connection closed")
                sockets.remove(ws)


async def start_server():
    print("Starting server on port 8001...")

    async def serve_html(request):
        return web.FileResponse(HTML_FILE)
    
    async def websocket_handler(request):
        print("Connected")
        ws = web.WebSocketResponse()
        sockets.append(ws)
        await ws.prepare(request)
        async for msg in ws:
            print("Received message: ", msg)
            pass
        return ws

    app = web.Application()
    app.add_routes([web.get('/', serve_html),
                    web.get('/ws', websocket_handler)])
    
    runner = web.AppRunner(app)
    await runner.setup()
    site = web.TCPSite(runner, '', 8001)
    await site.start()
    print("Server started")
    await asyncio.Future()  


def print_devices():
    for d in hid.enumerate():
        keys = list(d.keys())
        keys.sort()
        for key in keys:
            print("%s : %s" % (key, d[key]))

async def main():
    await asyncio.gather(start_server(), forward_inputs())


if (__name__ == "__main__"):
    loop = asyncio.get_event_loop()
    loop.run_until_complete(main())
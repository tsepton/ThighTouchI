
import hid
import asyncio
from aiohttp import web


VENDOR_ID = 1351
PRODUCT_ID = 12291
HTML_FILE = 'index.html'

async def main():
    try:
        device = hid.device()
        device.open(VENDOR_ID, PRODUCT_ID)  # TREZOR VendorID/ProductID

    except IOError as ex:
        print(ex)
        print("hid error:")
        print(device.error())

    async def serve_html(request):
        return web.FileResponse(HTML_FILE)
    
    async def websocket_handler(request):
        ws = web.WebSocketResponse()
        await ws.prepare(request)

        async def forward_inputs():
            while True:
                try:
                    data = str(device.read(64))
                    await ws.send_str(data)
                except Exception as e:
                    print(e)
                    break

        asyncio.create_task(forward_inputs())

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

    print("Server started on port 8001")
    await asyncio.Future()  


def print_devices():
    for d in hid.enumerate():
        keys = list(d.keys())
        keys.sort()
        for key in keys:
            print("%s : %s" % (key, d[key]))


if (__name__ == "__main__"):
    print_devices()
    asyncio.run(main())
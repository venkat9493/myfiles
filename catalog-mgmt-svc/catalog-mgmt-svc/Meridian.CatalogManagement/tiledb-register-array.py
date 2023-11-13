import tiledb
import numpy as np
import tiledb.cloud
import sys


config = tiledb.Config()
tiledb.cloud.client.client.retry_mode("disabled")
config["rest.server_address"]=sys.argv[3]
config["rest.token"]= sys.argv[4]
tiledb.cloud.login(host=sys.argv[3], token=sys.argv[4])
tiledb.cloud.file.create_file(
    name = sys.argv[6],
    namespace=sys.argv[5],   
    input_uri=sys.argv[1],
    output_uri=sys.argv[2]
)
args = dict([arg.split('=', maxsplit=1) for arg in sys.argv[7:]])
pathSplit = sys.argv[2].split('/')
array_name = 'tiledb://' + sys.argv[5] + '/' + pathSplit[len(pathSplit) - 1]
with tiledb.Array(array_name, "w", ctx=tiledb.cloud.Ctx()) as A:
    for x, y in args.items():
        A.meta[x] = y

import tiledb
import numpy as np
import tiledb.cloud
import sys


config = tiledb.Config()
config["rest.server_address"]=sys.argv[1]
config["rest.token"]= sys.argv[2]
tiledb.cloud.login(host=sys.argv[1], token=sys.argv[2])
# List all arrays with matching with a metadata key of tile_capacity
arrays = tiledb.cloud.list_arrays(namespace="nanostring",search=sys.argv[3])

# Print URIs of matching arrays
for array in arrays.arrays:
    print(f"Array {array.tiledb_uri} matached search")
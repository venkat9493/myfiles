import tiledb
import numpy as np
import tiledb.cloud
import sys


config = tiledb.Config()
tiledb.cloud.client.client.retry_mode("disabled")
config["rest.server_address"]="http://tdbeval1.tdbeval.online"
config["rest.token"]="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNjYyMzI4MGQtNTAwNi00N2Q4LTk2ZjctZmIxMjAyOWZjMzY2IiwiU2VlZCI6NTA4OSwiZXhwIjoxNjQwOTc1Mzk5LCJpYXQiOjE2Mzc4MzM1NDYsIm5iZiI6MTYzNzgzMzU0Niwic3ViIjoibWFuaWFwcGFuIn0.hbSTl4RYyebE3N8lYbNtT9Zg3ysw4U0OvOeY7vGTxNU"
print(sys.argv[1])
print(sys.argv[2])
tiledb.cloud.login(host="http://tdbeval1.tdbeval.online", token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNjYyMzI4MGQtNTAwNi00N2Q4LTk2ZjctZmIxMjAyOWZjMzY2IiwiU2VlZCI6NTA4OSwiZXhwIjoxNjQwOTc1Mzk5LCJpYXQiOjE2Mzc4MzM1NDYsIm5iZiI6MTYzNzgzMzU0Niwic3ViIjoibWFuaWFwcGFuIn0.hbSTl4RYyebE3N8lYbNtT9Zg3ysw4U0OvOeY7vGTxNU")
tiledb.cloud.file.create_file(
    namespace="maniappan",   
    input_uri=sys.argv[1],
    output_uri=sys.argv[2]
)
args = dict([arg.split('=', maxsplit=1) for arg in sys.argv[3:]])
pathSplit = sys.argv[2].split('/')
array_name = 'tiledb://maniappan/' + pathSplit[len(pathSplit) - 1]
with tiledb.Array(array_name, "w", ctx=tiledb.cloud.Ctx()) as A:
    for x, y in args.items():
        A.meta[x] = y

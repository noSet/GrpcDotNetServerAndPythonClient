from __future__ import print_function
import logging

import grpc

import quotes_pb2
import quotes_pb2_grpc


def run():
    with grpc.insecure_channel('127.0.0.1:8000') as channel:
        stub = quotes_pb2_grpc.QuotesServerStub(channel)
        for feature in stub.AccessToTheMarket(quotes_pb2.Subscription(quoteType = '债券')):
            print(feature.message)

if __name__ == '__main__':
    logging.basicConfig()
    run()

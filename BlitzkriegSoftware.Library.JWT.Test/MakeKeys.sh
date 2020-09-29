#!/bin/bash
openssl genrsa -f4 -out rsa4096.private 4096
openssl rsa -in rsa4096.private -outform PEM -pubout -out rsa4096.public

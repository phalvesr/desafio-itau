import os


def run():
    os.system('echo Starting containers on deamon mode...')
    os.chdir('./Docker')
    os.system('docker-compose up -d')
    os.system('echo Containers have been created!')


if __name__ == '__main__':
    run()

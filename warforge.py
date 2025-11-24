import argparse
import subprocess


def march(warriors: list, rearm: bool = False):
    command = ["docker", "compose", "up", "-d"]

    if rearm:
        command.append("--force-recreate")

    if warriors:
        command.extend(warriors)

    try:
        subprocess.run(command, check=True)
        print("March is going. Waaagh!!")

    except subprocess.CalledProcessError as e:
        print(f"Eugh, march gone bad... {e}")

    except FileNotFoundError:
        print("Where're warriors ?? Check yo compose files")


def slay(warriors: list):
    if warriors:
        for warrior in warriors:
            command = ["docker", "compose", "stop", warrior]
            try:
                subprocess.run(command, check=True)
                print(f"Slain {warrior}. Honor the fallen..")

            except subprocess.CalledProcessError as e:
                print(f"Oh no, {warrior} unstoppable!! {e}")

            except FileNotFoundError:
                print("Docker Compose not found. Make sure Docker is installed and in PATH.")
    else:
        command = ["docker", "compose", "down"]
        try:
            subprocess.run(command, check=True)
            print("No more waaagh.. all warriors gone home")

        except subprocess.CalledProcessError as e:
            print(f"Army is unstoppable! {e}")

        except FileNotFoundError:
            print("Where're warriors ?? Check yo compose files")



def main():
    parser = argparse.ArgumentParser(
        description="Warforge — summon or slay your docker warriors."
    )

    subparsers = parser.add_subparsers(dest="command", required=True)

    march_parser = subparsers.add_parser(
        "march", help="May your warriors bring you honor!"
    )
    slay_parser = subparsers.add_parser(
        "slay", help="Slay your warriors to Valhalla!"
    )

    march_parser.add_argument(
        "-w", "--warriors",
        nargs="*",
        required=False,
        help="Names of warriors (services) to summon. If omitted, all will march."
    )
    march_parser.add_argument(
        "-r", "--rearm",
        action="store_true",
        help="Fully rearm your warriors (force recreate containers)"
    )

    slay_parser.add_argument(
        "-w", "--warriors",
        nargs="*",
        required=False,
        help="Names of warriors (services) to slay. If omitted, all will be slain."
    )

    args = parser.parse_args()

    if args.command == "march":
        march(args.warriors, rearm=args.rearm)
    elif args.command == "slay":
        slay(args.warriors)

if __name__ == "__main__":
    main()

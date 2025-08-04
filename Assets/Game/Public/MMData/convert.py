import os

def convert_bvh_to_txt(input_dir, output_dir):
    # Create the output directory if it doesn't exist
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    # Iterate over all files in the input directory
    for filename in os.listdir(input_dir):
        if filename.endswith('.bvh'):
            input_filepath = os.path.join(input_dir, filename)
            output_filepath = os.path.join(output_dir, filename.replace('.bvh', '.txt'))

            # Read the content of the .bvh file
            with open(input_filepath, 'r') as bvh_file:
                content = bvh_file.read()

            # Write the content to the .txt file
            with open(output_filepath, 'w') as txt_file:
                txt_file.write(content)

            print(f'Converted {filename} to {output_filepath}')
